namespace eCom.Web.Models
{

    public class RoleDTO
    {
        public RoleDTO()
        {
            RolesList = [];
        }
        public UserDTO User { get; set; }
        public List<RoleSelection> RolesList { get; set; }
    }

    public class RoleSelection
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }

    }

}
