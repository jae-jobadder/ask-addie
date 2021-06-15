using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Google.Cloud.Dialogflow.V2.Intent.Types.Message.Types;

namespace AskAddie.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class FinancialController : BaseDialogFlowController
  {
    public FinancialController() { }

    [HttpPost("report")]
    public IActionResult Report()
    {
      WebhookRequest request;
      using (var reader = new StreamReader(Request.Body))
      {
        request = JsonParser.Parse<WebhookRequest>(reader);
      }

      WebhookResponse response = new WebhookResponse 
      {
        FulfillmentText = "This works; does this pause?;" 
      };


      string responseJson = response.ToString();
      return Content(responseJson, "application/json");
    }
  }
}
