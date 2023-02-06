using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

public partial class Program
{
    private static IConfigurationRefresher _refresher = null;


    public static void Main(string[] args)
    {
                
        Console.WriteLine("Hello, World!");

        //重複分は後で上書き
        var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(path: "local.appsettings.json")
        .AddEnvironmentVariables(prefix: "DOTNET_");
        var config = builder.Build();


        //最初にlabelがないやつを読み込み、そのあとにlabelがdevのものを読み込む。同じ設定は後勝ちで上書きされる
        //ホスト名のprefixとかでdevとかprdを判断できるといいかも
        builder.AddAzureAppConfiguration(options =>{
            options.Connect(config["ConnectionString"])
            .ConfigureRefresh(refresh => {
            refresh.Register(KeyFilter.Any).SetCacheExpiration(TimeSpan.FromSeconds(10));
            })
            .Select(KeyFilter.Any,LabelFilter.Null)
            .Select(KeyFilter.Any,"dev");

            _refresher = options.GetRefresher();
        });
        config = builder.Build();
        Console.WriteLine(config.GetDebugView());
        var setting2 = config.GetSection("test").Get<appSetting>();

        var con = (config.GetSection("test").GetSection("etc"))["sample1"];
        Console.WriteLine(con);

    }
}


public class appSetting{
    public etc etc{get;set;}
    public etc foo{get;set;}
}

public class etc{
    public string sample1 {get;set;}
    public string sample2 {get;set;}
}