namespace MasterServiceBack.Dto;

public class ResponseDto
{
    public ResponseDto()    {
        this.Code = 0;      this.Message = "Success";
    }
    public ResponseDto(int code, string message)    {
        this.Code = code;      this.Message = message;
    }
    /// <summary>Indicates successful of the response</summary>    /// <example>0</example>
    public int Code { get; set; }
    /// <summary>Additional message</summary>    /// <example>Success</example>
    public string Message { get; set; }  
}