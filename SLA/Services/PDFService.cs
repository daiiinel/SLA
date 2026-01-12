namespace SLA.Services;

public class PDFService
{
    public static string GenerarHtmlResumen(Models.Registro registro)
    {
        var filasItems = ""; //dibujito del form 2404 con mi html/css precioso (ya estamos en mi terreno)
        foreach (var item in registro.Items)
        {
            filasItems += $@"
                <tr>
                    <td style='text-align: center;'>{item.Tipo}</td>
                    <td>{item.Descripcion}</td>
                    <td style='text-align: center;'>{item.Cantidad}</td>
                </tr>";
        }

        return $@"
            <html>
            <head>
                <style>
                    body {{ font-family: 'Courier New', Courier, monospace; color: #333; }}
                    .container {{ border: 2px solid #000; padding: 10px; }}
                    .header {{ text-align: center; border-bottom: 2px solid #000; margin-bottom: 15px; padding-bottom: 10px; }}
                    .info-section {{ display: flex; flex-wrap: wrap; margin-bottom: 20px; border-bottom: 1px solid #000; padding-bottom: 10px; }}
                    .info-box {{ width: 50%; margin-bottom: 5px; font-size: 14px; }}
                    table {{ width: 100%; border-collapse: collapse; margin-top: 10px; }}
                    th {{ background-color: #f2f2f2; border: 1px solid #000; padding: 10px; text-transform: uppercase; font-size: 12px; }}
                    td {{ border: 1px solid #000; padding: 8px; font-size: 13px; }}
                    .footer {{ margin-top: 30px; border-top: 1px solid #000; padding-top: 10px; font-size: 11px; }}
                    .signature-space {{ margin-top: 60px; display: flex; justify-content: space-around; }}
                    .sig-line {{ border-top: 1px solid #000; width: 200px; text-align: center; padding-top: 5px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2 style='margin:0;'>SISTEMA DE LOGÍSTICA AUTOMATIZADO (SLA)</h2>
                        <h3 style='margin:5px 0;'>REGISTRO DE MOVIMIENTO DE MATERIAL - FORMULARIO 2404</h3>
                    </div>

                    <div class='info-section'>
                        <div class='info-box'><b>ID:</b> {registro.Id.ToString().Substring(0, 8).ToUpper()}</div>
                        <div class='info-box'><b>FECHA:</b> {registro.Fecha:dd/MM/yyyy HH:mm}</div>
                        <div class='info-box'><b>UNIDAD/DEP:</b> {registro.Unidad.ToUpper()}</div>
                        <div class='info-box'><b>MOVIMIENTO:</b> {registro.TipoMovimiento.ToUpper()}</div>
                        <div class='info-box'><b>OPERADOR:</b> {registro.Operador.ToUpper()}</div>
                    </div>

                    <table>
                        <thead>
                            <tr>
                                <th style='width: 20%;'>Clasificación</th>
                                <th style='width: 65%;'>Descripción del Material / Nro. Serie</th>
                                <th style='width: 15%;'>Cantidad</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filasItems}
                        </tbody>
                    </table>

                    <div class='footer'>
                        <b>OBSERVACIONES:</b> {registro.Observaciones ?? "SIN NOVEDAD"}<br><br>
                        <i>Este documento es una representación digital válida de los registros de almacén.</i>
                    </div>

                    <div class='signature-space'>
                        <div class='sig-line'>Firma del Operador</div>
                        <div class='sig-line'>V° B° Jefe de Sección</div>
                    </div>
                </div>
            </body>
            </html>";
    }
}