import { useEffect, useMemo, useState } from "react";
import { bookClient } from "./baseUrl.ts";
import type { BookDto, CreateBookRequestDto } from "./generated-ts-client.ts";

function App() {
    const [books, setBooks] = useState<BookDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [err, setErr] = useState<string | null>(null);

    const [query, setQuery] = useState("");
    const [creating, setCreating] = useState(false);

    const [form, setForm] = useState<CreateBookRequestDto>({
        title: "",
        pages: 150,
    });

    useEffect(() => {
        refresh();
    }, []);

    async function refresh() {
        try {
            setLoading(true);
            setErr(null);
            const data = await bookClient.getAllBooks();
            setBooks(Array.isArray(data) ? data : []);
        } catch (e: any) {
            setErr(e?.message ?? "Failed to load books");
        } finally {
            setLoading(false);
        }
    }

    const canCreate =
        form.title.trim().length > 0 &&
        Number.isFinite(form.pages as number) &&
        (form.pages as number) > 0 &&
        !creating;

    async function handleCreate() {
        if (!canCreate) return;
        try {
            setCreating(true);

            const tempId = `temp_${Math.random().toString(36).slice(2)}`;
            const optimistic: BookDto = {
                id: tempId,
                title: form.title.trim(),
                pages: form.pages,
                createdAt: new Date().toISOString(),
            };
            setBooks((prev) => [optimistic, ...prev]);

            const created = await bookClient.createBook({
                title: optimistic.title!,
                pages: optimistic.pages ?? 0,
            });

            setBooks((prev) => prev.map((b) => (b.id === tempId ? created : b)));
            setForm({ title: "", pages: 150 });
        } catch (e: any) {
            setBooks((prev) => prev.filter((b) => !String(b.id).startsWith("temp_")));
            alert(e?.message ?? "Failed to create book");
        } finally {
            setCreating(false);
        }
    }

    async function handleDelete(id?: string) {
        if (!id) return;
        if (!window.confirm("Delete this book?")) return;

        const prev = books;
        setBooks((cur) => cur.filter((b) => b.id !== id));
        try {
            await bookClient.deleteBook(id);
        } catch (e: any) {
            setBooks(prev);
            alert(e?.message ?? "Failed to delete book");
        }
    }

    const filtered = useMemo(() => {
        const q = query.trim().toLowerCase();
        if (!q) return books;
        return books.filter(
            (b) =>
                (b.title ?? "").toLowerCase().includes(q) ||
                (b.id ?? "").toLowerCase().includes(q)
        );
    }, [books, query]);

    return (
        <div className="min-h-dvh bg-base-200">
            {/* Compact, subtle header (fixed) */}
            <div className="navbar fixed left-0 right-0 top-0 z-50 bg-base-100/80 backdrop-blur border-b border-base-300 min-h-12">
                <div className="container mx-auto px-3 md:px-4 flex items-center justify-between relative">
                    {/* Left: title */}
                    <div className="flex items-center">
                        <span className="font-semibold text-base leading-none">📚 Library</span>
                    </div>

                    {/* Center: fun text */}
                    <div className="absolute left-1/2 -translate-x-1/2 text-xs md:text-sm text-base-content/70 whitespace-nowrap select-none pointer-events-none">
                        this was vibe-coded in 20 minutes just to show the API requests work
                    </div>

                    {/* Right: search + refresh */}
                    <div className="flex items-center gap-2">
                        <input
                            type="text"
                            className="input input-sm input-bordered w-40 md:w-64"
                            placeholder="Search title or ID…"
                            value={query}
                            onChange={(e) => setQuery(e.target.value)}
                            aria-label="Filter books"
                        />
                        <button className="btn btn-sm" onClick={refresh}>
                            Refresh
                        </button>
                    </div>
                </div>
            </div>

            {/* Main */}
            <main className="container mx-auto px-3 md:px-4 pt-16 pb-8">
                {/* Create */}
                <section className="card bg-base-100/90 border border-base-300 shadow-sm">
                    <div className="card-body p-4 md:p-5">
                        <h2 className="card-title text-base font-semibold">Add a book</h2>
                        <div className="grid grid-cols-1 sm:grid-cols-[1fr_120px_auto] gap-2 md:gap-3">
                            <input
                                className="input input-sm input-bordered"
                                value={form.title}
                                onChange={(e) => setForm((f) => ({ ...f, title: e.target.value }))}
                                placeholder="Title"
                                aria-label="Book title"
                            />
                            <input
                                className="input input-sm input-bordered"
                                type="number"
                                min={1}
                                value={form.pages ?? ""}
                                onChange={(e) =>
                                    setForm((f) => ({ ...f, pages: Number(e.target.value) || 0 }))
                                }
                                placeholder="Pages"
                                aria-label="Page count"
                            />
                            <button
                                className={`btn btn-sm btn-primary ${creating ? "loading" : ""}`}
                                onClick={handleCreate}
                                disabled={!canCreate}
                            >
                                {creating ? "Creating" : "Create"}
                            </button>
                        </div>
                    </div>
                </section>

                {/* States */}
                {loading && (
                    <section className="mt-4 grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-5 gap-3 md:gap-4">
                        <SkeletonCards />
                    </section>
                )}

                {err && !loading && (
                    <section className="mt-4 alert alert-error shadow-sm">
                        <div>
                            <span className="font-medium">Couldn’t load books.</span>
                            <div className="text-xs opacity-80">{err}</div>
                        </div>
                    </section>
                )}

                {!loading && !err && filtered.length === 0 && (
                    <section className="mt-4 card bg-base-100/70 border border-dashed border-base-300">
                        <div className="card-body items-center text-center p-6">
                            <h3 className="font-semibold">No books yet</h3>
                            <p className="text-sm text-base-content/70">
                                Use “Add a book” above to create your first one.
                            </p>
                        </div>
                    </section>
                )}

                {!loading && !err && filtered.length > 0 && (
                    <section className="mt-4 grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-5 gap-3 md:gap-4">
                        {filtered.map((b) => (
                            <BookCard key={b.id ?? cryptoKey()} book={b} onDelete={handleDelete} />
                        ))}
                    </section>
                )}
            </main>
        </div>
    );
}

function BookCard({
                      book,
                      onDelete,
                  }: {
    book: BookDto;
    onDelete: (id?: string) => void;
}) {
    const created = book.createdAt
        ? new Date(book.createdAt).toLocaleString()
        : "—";

    return (
        <article className="card bg-base-100/90 border border-base-300 shadow-sm hover:shadow-md transition-shadow">
            <div className="card-body p-4">
                <div className="flex items-center justify-between gap-3">
                    <h3
                        className="font-semibold text-sm md:text-base truncate"
                        title={book.title || "(untitled)"}
                    >
                        {book.title || "(untitled)"}
                    </h3>
                    <div className="badge badge-ghost text-xs">{book.pages ?? 0} pages</div>
                </div>

                <div className="mt-1 space-y-1 text-xs text-base-content/70">
                    <div>
                        <span className="font-medium text-base-content/60">ID:</span>{" "}
                        {book.id ?? "—"}
                    </div>
                    <div>
                        <span className="font-medium text-base-content/60">Created:</span>{" "}
                        {created}
                    </div>
                    <div>
                        <span className="font-medium text-base-content/60">Authors:</span>{" "}
                        {Array.isArray(book.authorIds) && book.authorIds.length > 0
                            ? book.authorIds.join(", ")
                            : "—"}
                    </div>
                    <div>
                        <span className="font-medium text-base-content/60">Genre ID:</span>{" "}
                        {book.genreId ?? "—"}
                    </div>
                </div>

                <div className="card-actions justify-end mt-3">
                    <button
                        className="btn btn-xs md:btn-sm btn-outline btn-error"
                        onClick={() => onDelete(book.id)}
                    >
                        Delete
                    </button>
                </div>
            </div>
        </article>
    );
}

function SkeletonCards() {
    return (
        <>
            {Array.from({ length: 8 }).map((_, i) => (
                <div key={i} className="card bg-base-100/90 border border-base-300 shadow-sm">
                    <div className="card-body p-4">
                        <div className="skeleton h-4 w-3/5" />
                        <div className="skeleton h-3 w-2/5 mt-2" />
                        <div className="skeleton h-3 w-4/5 mt-2" />
                        <div className="skeleton h-7 w-20 mt-4" />
                    </div>
                </div>
            ))}
        </>
    );
}

/** key helper when id is missing */
function cryptoKey() {
    return Math.random().toString(36).slice(2);
}

export default App;
