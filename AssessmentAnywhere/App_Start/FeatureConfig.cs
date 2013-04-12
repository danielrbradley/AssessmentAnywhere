namespace AssessmentAnywhere.App_Start
{
    using AssessmentAnywhere.Features;

    using Autofac;

    public class FeatureConfig
    {
        public static void RegisterFeatures(ContainerBuilder builder)
        {
            builder.Register(c => new AccountFeatures(requireActivation: false));
        }
    }
}
