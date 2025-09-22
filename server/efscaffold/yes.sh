dotnet ef dbcontext scaffold "Host=ep-long-cake-ag9gfm64-pooler.c-2.eu-central-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_xzVEM8eOdIK1;Ssl Mode=Require;Trust Server Certificate=true;ChannelBinding=Require" Npgsql.EntityFrameworkCore.PostgreSQL `
    --output-dir ./Entities `
    --context-dir . `
    --context MyDbContext `
    --no-onconfiguring `
    --namespace efscaffold.Entities `
    --context-namespace Infrastructure.Postgres.Scaffolding `
    --schema library `
    --force