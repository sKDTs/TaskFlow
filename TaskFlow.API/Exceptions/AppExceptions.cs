namespace TaskFlow.API.Exceptions;

/// <summary>Recurso não encontrado -> 404</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

/// <summary>Violação de regra de negócio / validação -> 400</summary>
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}
