namespace WAppMarvelComics.API.Models.DTOs
{
    public class ApiResponseDto<T>
    {
        public ApiResponseDto(T data)
        {
            Data = data;
            IsSuccess = true;
            ReturnMessage = "";
        }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string ReturnMessage { get; set; }
    }
}