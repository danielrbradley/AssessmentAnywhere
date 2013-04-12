namespace AssessmentAnywhere.Services
{
    using Autofac;

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AutofacModule).Assembly).AsImplementedInterfaces();
        }
    }
}
