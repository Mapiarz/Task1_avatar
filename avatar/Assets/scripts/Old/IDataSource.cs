/// <summary>
/// an interface providing an universal method for getting data from multiple, different sources
/// </summary>
public interface IDataSource
{
    DataFrame GetData();
}
