using System;
using System.ComponentModel.DataAnnotations;
namespace HRMSPOC.WEB.Models;
public class OrganizationViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Organization name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; }
}
