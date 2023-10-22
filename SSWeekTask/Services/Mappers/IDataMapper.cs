namespace SSWeekTask.Services.Mappers;

public interface IDataMapper
{
    /// <summary>
    /// Deep copy
    /// </summary>
    TSource Clone<TSource>(TSource source);

    /// <summary>
    /// Mappes one type to another
    /// </summary>
    TDes Map<TSource, TDes>(TSource source);

    /// <summary>
    /// Mappes one type to another
    /// </summary>
    TDes Map<TDes>(object source);
}
