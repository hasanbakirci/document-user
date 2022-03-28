using Microsoft.AspNetCore.Routing.Constraints;

namespace document_service.Helpers;

public class FileHelper
{
    public static Task<FileResponse> Add(IFormFile file){
        var sourcepath = Path.GetTempFileName();

        if(file.Length > 0){
            using(var stream = new FileStream(sourcepath, FileMode.Create)){
                file.CopyTo(stream);
            }
        }
        var result = newPath(file);
        File.Move(sourcepath, result.FilePath);
        return Task.FromResult(result);
    }

    private static FileResponse newPath(IFormFile file)
    {
        FileInfo ff = new FileInfo(file.FileName);
        
        string fileExtension = ff.Extension;

        string path = Environment.CurrentDirectory + @"\Documents";
        var newPath = Guid.NewGuid().ToString() + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + fileExtension;

        string result = $@"{path}\{newPath}";
        
        return new FileResponse
        {
            FileExtension = fileExtension,
            FileName = newPath,
            FilePath = result
        };
    }
}

public class FileResponse
{
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public string FilePath { get; set; }
}