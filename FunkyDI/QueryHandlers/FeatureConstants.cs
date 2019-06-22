using System;

namespace FunkyDI.QueryHandlers
{
    public static class FeatureConstants
    {
        public const string AllowedFeaturesClaim = "allowedfeatures";
        public static Guid Customers { get; private set; }
        public static Guid Managers { get; private set; }

        static FeatureConstants()
        {
            Customers = Guid.NewGuid();
            Managers = Guid.NewGuid();
        }
    }
}