
public class EmailIDValidation 
{
    public string UserId { get; set; }
    public string MailId { get; set; }
}

public class Validesponse
{
    public string Status { get; set; }
    public string Message { get; set; }
}

public class PasswordUpdate 
{
    public string UserId { get; set; }
    public string Password { get; set; }
}

public class PasswordResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
}