namespace SLA.Models;

public enum EstadoRegistro
{
    Borrador, //en proceso
    Entregado, //acciona operador
    Auditado, //acciona auditor
    Rechazado
}