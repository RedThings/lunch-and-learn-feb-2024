using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RealtimeRating.WebUi.Pages;

public class NewQuoteModel : PageModel
{
    public Guid CustomerId { get; private set; }

    public void OnGet()
    {
        CustomerId = Guid.Parse(Request.Query["customer_id"].Single() ?? string.Empty);
    }
}