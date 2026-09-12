namespace WebArMa.ArMaMelk.Web.Application._Shared;

public class ApiResult<T>
{
	public bool IsSuccess { get; init; }
	public T? Data { get; init; }
	public string? ErrorMessage { get; init; }

	public static ApiResult<T> Success(T data) => new() { IsSuccess = true, Data = data };
	public static ApiResult<T> Failure(string errorMessage) => new() { IsSuccess = false, ErrorMessage = errorMessage };
}