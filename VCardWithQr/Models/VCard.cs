namespace VCardWithQr.Models;

public class VCard
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Company { get; set; }

    public string ToVCard()
    {
        return $"BEGIN:VCARD\n" +
               $"VERSION:3.0\n" +
               $"FN:{FirstName} {LastName}\n" +
               $"ORG:{Company}\n" +
               $"TEL;TYPE=WORK,VOICE:{PhoneNumber}\n" +
               $"ADR;TYPE=WORK:;;{Address}\n" +
               $"EMAIL:{Email}\n" +
               $"END:VCARD";
    }
}
