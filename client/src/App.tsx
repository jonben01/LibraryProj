import {useEffect, useState} from "react";
import {bookClient} from "./baseUrl.ts";
import type {BookDto, CreateBookRequestDto} from "./generated-ts-client.ts";


function App() {

    const [books, setBooks] = useState<BookDto[]>([]);
    const [myForm, setForm] = useState<CreateBookRequestDto>({
        pages: 150,
        title: ''

    });

    useEffect(() => {
        bookClient.getAllBooks().then(r => {
            setBooks(r);
        })

    }, [])
    

  return (
    <>
        <input value={myForm.title} onChange={c => setForm({...myForm, title: c.target.value})} placeholder={"Book Title"}/>
        <input value={myForm.pages} placeholder={"Page Number"}/>
        <button onClick={() => {

        }}>Create new book</button>


        {
            books.map(book => {
                return <div key={book.id}>
                    {JSON.stringify(book)}
                </div>
            })
        }
    </>
  )
}

export default App
