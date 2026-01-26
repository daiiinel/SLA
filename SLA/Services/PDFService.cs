namespace SLA.Services;

public class PDFService
{
    public static async Task<string> GenerarHtmlResumen(Models.Registro registro, string firmaBase64)
    {
        string html="";
        using (var stream = await FileSystem.OpenAppPackageFileAsync("2404.html"))
            using (var reader = new StreamReader(stream))
                html = await reader.ReadToEndAsync();

        string firmaOperadorBase64 = Preferences.Get("FirmaOperadorBase64", string.Empty);

        var filasItems = ""; //dibujito del form 2404 con mi html/css precioso (ya estamos en mi terreno)
        foreach (var item in registro.Items)
        {
            filasItems += $@"
                <tr>
                    <td style='text-align: center; border: 1px solid #444;'>{item.Tipo}</td>
                    <td style='border: 1px solid #444; padding: 5px;'>{item.Descripcion}</td>
                    <td style='text-align: center; border: 1px solid #444;'>{item.Cantidad}</td>
                </tr>";
        }

        string htmlFinal = html
            .Replace("{{ID}}", registro.Id.ToString().Substring(0, 8).ToUpper())
            .Replace("{{FECHA}}", registro.Fecha.ToString("dd/MM/yyyy HH:mm"))
            .Replace("{{UNIDAD}}", registro.Unidad.ToUpper())
            .Replace("{{MOVIMIENTO}}", registro.TipoMovimiento.ToUpper())
            .Replace("{{OPERADOR}}", registro.Operador.ToUpper())
            //nuevos campos :n
            .Replace("{{RECEPTOR_NOMBRE}}", registro.NombreCompletoReceptor?.ToUpper())
            .Replace("{{RECEPTOR_DNI}}", registro.BusquedaDNI ?? "---")
            .Replace("{{RECEPTOR_GRADO}}", registro.GradoUnidadReceptor?.ToUpper() ?? "---")
             //firma
             .Replace("{{FIRMA_RECEPTOR}}", $"<img src='data:image/png;base64,{firmaBase64}' class='firma-img' />")
            //firma op (la guardada por def)
            .Replace("{{FIRMA_OPERADOR}}", !string.IsNullOrEmpty(registro.FirmaOperadorBase64)
                ? $"<img src='data:image/png;base64,{registro.FirmaOperadorBase64}' class='firma-img' />"
                : "<p style='font-size:10px;'>Sin firma registrada</p>")
            //
            .Replace("{{ESTADO}}", registro.Estado.ToString().ToUpper())
            .Replace("{{OBSERVACIONES}}", registro.Observaciones ?? "SIN NOVEDAD")
            .Replace("{{FILAS_ITEMS}}", filasItems);
    
        return htmlFinal;
    }
}