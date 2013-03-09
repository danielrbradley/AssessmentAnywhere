namespace AssessmentAnywhere.Services.Repos.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public static class GradeBoundariesExtensions
    {
        public static string ForResult(this IEnumerable<Boundary> boundaries, decimal? result)
        {
            if (result.HasValue)
            {
                var match =
                    boundaries.Where(boundary => result.Value >= boundary.MinResult)
                              .OrderByDescending(boundary => boundary.MinResult)
                              .FirstOrDefault();
                if (match != null)
                {
                    return match.Grade;
                }
            }

            return string.Empty;
        }
    }
}
