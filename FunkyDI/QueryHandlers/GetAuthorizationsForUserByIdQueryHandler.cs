using System.Collections.Generic;
using System.Threading.Tasks;
using FunkyDI.DTO;

namespace FunkyDI.QueryHandlers
{
    public class GetAuthorizationsForUserByIdQueryHandler : IQueryHandler<GetAuthorizationsForUserByIdQuery, AllowedFeatureCollection>
    {
        public Task<AllowedFeatureCollection> HandleAsync(GetAuthorizationsForUserByIdQuery query)
        {
            if (query == null || query.Id <= 0)
            {
                return Task.FromResult<AllowedFeatureCollection>(null);
            }

            var id = query.Id;

            if (id % 2 == 0)
            {
                return Task.FromResult(new AllowedFeatureCollection
                {
                    Features = new List<ApplicationFeature>
                    {
                        new ApplicationFeature {FeatureId = FeatureConstants.Customers, FeatureName = "Customers"},
                        new ApplicationFeature {FeatureId = FeatureConstants.Managers, FeatureName = "Managers"}
                    }
                });
            }

            return Task.FromResult(new AllowedFeatureCollection
            {
                Features = new List<ApplicationFeature>
                {
                    new ApplicationFeature {FeatureId = FeatureConstants.Managers, FeatureName = "Managers"}
                }
            });
        }
    }
}