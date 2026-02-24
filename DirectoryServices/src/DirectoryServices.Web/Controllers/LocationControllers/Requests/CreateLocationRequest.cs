using DirectoryServices.Contracts;

namespace DirectoryServices.Web.Controllers.LocationControllers.Requests;

public record CreateLocationRequest(string LocationName, AddressDto AddressDto, string Timezone);

