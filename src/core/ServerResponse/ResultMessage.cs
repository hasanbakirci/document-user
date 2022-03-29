namespace core.ServerResponse;

public class ResultMessage
{
    public static string Success => "messages.success.general.ok";
    public static string Error => "messages.error.general.false";
    public static string InvalidModel => "messages.error.general.invalidModel";
    public static string UnhandledException => "messages.error.general.unhandledException";
    public static string UnAuthorized => "messages.error.general.unAuthorized";
    public static string Forbidden => "messages.error.general.Forbidden";

    // Document
    public static string NotFoundDocument => "messages.error.document.notFoundDocument";

    // User
    public static string NotFoundUser => "messages.error.user.notFoundUser";

}