using System;

namespace FunkyDI.QueryHandlers
{
    public static class FeatureConstants
    {
        public const string AllowedFeaturesClaim = "allowedfeatures";

        static FeatureConstants()
        {
            Customers = Guid.NewGuid();
            Managers = Guid.NewGuid();
        }

        public static Guid Customers { get; }
        public static Guid Managers { get; }
    }
}