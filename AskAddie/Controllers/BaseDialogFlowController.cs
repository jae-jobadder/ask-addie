using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;

namespace AskAddie.Controllers
{
  public class BaseDialogFlowController : ControllerBase
  {
    // A Protobuf JSON parser configured to ignore unknown fields. This makes
    // the action robust against new fields being introduced by Dialogflow.
    public readonly JsonParser JsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

    public BaseDialogFlowController()
    {
      
    }
  }
}
