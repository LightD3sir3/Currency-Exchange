public class ExceptionHandlerService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public event Func<Exception, Task> OnException;

    public ExceptionHandlerService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void HandleException(Exception ex)
    {
        Console.WriteLine(ex);
        OnException?.Invoke(ex);
    }

    //public void HandleException(Exception ex)
    //{
    //    using var scope = _serviceScopeFactory.CreateScope();
    //    var dialogService = scope.ServiceProvider.GetRequiredService<DialogService>();

    //    dialogService.Open<ExceptionDialog>(title: "Error", parameters: new Dictionary<string, object>() { { "Message", ex.Message } }, options: new DialogOptions() { Width = "300px", Height = "200px" });
    //}
}
