using Mapster;
using MapsterMapper;
using FastExpressionCompiler;
using SSWeekTask.Data.Entities;
using SSWeekTask.Models;

namespace SSWeekTask.Services.Mappers;

public class DataMapper : IDataMapper
{
    private readonly Lazy<Mapper> _localMapper;
    public Mapper LocalMapper => _localMapper.Value;
    private static DataMapper _instance;
    private static TypeAdapterConfig _cfg;

    public static DataMapper Current => _instance ??= new DataMapper();

    public DataMapper()
    {
        _localMapper = new Lazy<Mapper>(() => new Mapper(_cfg ??= Config()));
    }
    public TSource Clone<TSource>(TSource source)
    {
        return LocalMapper.Map<TSource, TSource>(source);
    }

    public TDes Map<TSource, TDes>(TSource source)
    {
        return LocalMapper.Map<TSource, TDes>(source);
    }

    public TDes Map<TDes>(object source)
    {
        return LocalMapper.Map<TDes>(source);
    }

    private TypeAdapterConfig Config()
    {
        var cfg = TypeAdapterConfig.GlobalSettings;

        cfg.Compiler = static exp => exp.CompileFast();

        cfg.RequireDestinationMemberSource = true;
        cfg.Default.PreserveReference(true);

        RegisterDtoToDomainAndBack(cfg);

        try
        {
            cfg.Compile();
        }
        catch (Exception ex)
        {
        }

        return cfg;
    }

    private static void RegisterDtoToDomainAndBack(TypeAdapterConfig cfg)
    {
        cfg.NewConfig<StudentEntity, StudentDto>()
            .TwoWays();

        cfg.NewConfig<TeacherEntity, TeacherDto>()
            .TwoWays();

        cfg.NewConfig<CourseDto, CourseEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.TeacherId, src => src.TeacherId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Ignore(dest => dest.Teacher)
            .Ignore(dest => dest.Students)
            .TwoWays();
    }
}
