using System.Reflection;

namespace OpcClient.Infrastructure.Logic
{
    public interface ITypeConverter
    {
         object Convert(string type, string value);

         TypeInfo Convert(string type);
    }
}
