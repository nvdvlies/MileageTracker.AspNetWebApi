using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace MileageTracker.Extensions {
    public static class DbEntityValidationExceptionExtensions {
        public static string GetErrorResult(this DbEntityValidationException ex) {
            if (ex.EntityValidationErrors != null && ex.EntityValidationErrors.Any()) {
                var message = ex.EntityValidationErrors
                    .SelectMany(error => error.ValidationErrors)
                    .Aggregate(String.Empty, (errors, error) =>
                        errors +
                        String.Format("Property: {0} Error: {1}{2}", error.PropertyName,
                            error.ErrorMessage, Environment.NewLine)
                    );
                return message;
            }
            return ex.Message;
        }
    }
}