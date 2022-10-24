using System.Collections.Generic;
using System.Linq;

namespace Jwt.Identity.Core
{
    public class JwtIdentityResult
    {
        public bool Succeeded { get; protected set; }

        private List<JwtIdentityError> _errors = new List<JwtIdentityError>();

        public IEnumerable<JwtIdentityError> Errors => _errors;

        public static JwtIdentityResult Success { get; } = new JwtIdentityResult { Succeeded = true };

        public static JwtIdentityResult Failed(params JwtIdentityError[] errors)
        {
            var result = new JwtIdentityResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }

            return result;
        }

        public override string ToString()
        {
            return Succeeded ? "Succeeded" : $"{string.Join(",", Errors.Select(x => x.Code).ToList())} {string.Join(",", Errors.Select(x => x.Description).ToList())}";
        }
    }
}