using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Contracts;

public interface IModuleInstaller
{
    void Install(IServiceCollection services , IConfiguration configuration);
}