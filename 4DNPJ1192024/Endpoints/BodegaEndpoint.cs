namespace _4DNPJ1192024.Endpoints
{
    public static class BodegaEndpoint
    {
        static List<Bodega> bodegas = new List<Bodega>();

        public static void AddBodegaEndpoints(this WebApplication app)
        {

            app.MapPost("/bodegas", (BodegaRequest request) =>
            {

                if (request.Id <= 0)
                {
                    return Results.BadRequest("El Id debe ser un número positivo.");
                }

                var bodega = new Bodega
                {
                    Id = request.Id,
                    Nombre = request.Nombre,
                    Ubicacion = request.Ubicacion
                };

                bodegas.Add(bodega);

                return Results.Created($"/bodegas/{bodega.Id}", bodega);
            }).RequireAuthorization();


            app.MapPut("/bodegas/{id:int}", (int id, BodegaRequest request) =>
            {
                var bodega = bodegas.FirstOrDefault(b => b.Id == id);
                if (bodega == null) return Results.NotFound("Bodega no encontrada");

                bodega.Nombre = request.Nombre;
                bodega.Ubicacion = request.Ubicacion;

                return Results.Ok(bodega);
            }).RequireAuthorization();


            app.MapGet("/bodegas/{id:int}", (int id) =>
            {
                var bodega = bodegas.FirstOrDefault(b => b.Id == id);
                if (bodega == null) return Results.NotFound("Bodega no encontrada");

                return Results.Ok(bodega);
            }).RequireAuthorization();


            app.MapGet("/bodegas", () =>
            {
                return Results.Ok(bodegas);
            }).RequireAuthorization();
        }
    }

    public class Bodega
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Ubicacion { get; set; }
    }

    public class BodegaRequest
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Ubicacion { get; set; }
    }
}   


    






 
   