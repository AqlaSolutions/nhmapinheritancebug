using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Cfg;

public abstract class EntityBase
{
    public virtual int Id { get; set; }
}

public class TestEntity : EntityBase
{
    //public override int Id { get; set; } // uncomment this line and it works
    public virtual string SomeField { get; set; }
}

public class AutomappingConfiguration : DefaultAutomappingConfiguration
{
    public override bool ShouldMap(Type type)
    {
        return !type.IsAbstract && type.IsSubclassOf(typeof(EntityBase));
    }
}

public class Program
{
    public static void Main()
    {
        var configuration = new Configuration();

        configuration.Configure("hibernate.cfg.xml");

        Fluently.Configure(configuration)
                .Mappings(m => m.AutoMappings.Add(
                    AutoMap.AssemblyOf<Program>(new AutomappingConfiguration())
                    .Conventions.AddFromAssemblyOf<Program>()
                    .UseOverridesFromAssemblyOf<Program>()))
                .BuildConfiguration();
    }
}

public class NamingConventions : IPropertyConvention, IReferenceConvention, IIdConvention, IClassConvention, IHasManyConvention
{
    public void Apply(IPropertyInstance instance)
    {
        instance.Property.Name.GetHashCode();
    }

    public void Apply(IManyToOneInstance instance)
    {
        instance.Property.Name.GetHashCode();
    }

    public void Apply(IIdentityInstance instance)
    {
        instance.Property.Name.GetHashCode();
    }

    public void Apply(IClassInstance instance)
    {
        instance.EntityType.Name.GetHashCode();
    }

    public void Apply(IOneToManyCollectionInstance instance)
    {
        instance.EntityType.Name.GetHashCode();
    }
}

public class TestEntityOverride : IAutoMappingOverride<TestEntity>
{
    public virtual void Override(AutoMapping<TestEntity> mapping)
    {
        mapping.Id(x => x.Id);
    }
}