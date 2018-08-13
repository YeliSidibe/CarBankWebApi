using System.Collections.Generic;
using System.Net;

namespace CarBank.WebApi.Models
{
    public class AuthenticationResponse<T>
    {
        public HttpStatusCode StatusCode    { get; set; }
        public T Value                      { get; set; }
        public List<string> Errors          { get; set; }
        public bool succcess                { get { return Errors?.Count == 0; } }
    }
}
