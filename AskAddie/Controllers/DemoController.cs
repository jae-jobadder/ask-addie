﻿using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static Google.Cloud.Dialogflow.V2.Intent.Types.Message.Types;

namespace AskAddie.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class DemoController : ControllerBase
  {
    // A Protobuf JSON parser configured to ignore unknown fields. This makes
    // the action robust against new fields being introduced by Dialogflow.
    private static readonly JsonParser jsonParser =
    new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

    [HttpGet("")]
    public IActionResult Get()
    {
      return Ok("Response returned");
    }

    // Syntax Emaples
    private void Examples()
    {
      // Use FulfillmentMessages for rich responses
      WebhookResponse response = new WebhookResponse
      {
        // Cannot create a new instance of FulfillmentMessages
        // Must Add values as instead of assigning value as it is readonly
        FulfillmentMessages =
        {
          new Intent.Types.Message
          {
            Card = new Card
            {
              Title = "Title",
              // Same for button, if readonly, add values instead of assinging porperty
              Buttons =
              {
                new Card.Types.Button
                {
                  Text = "Button Text",
                  Postback = "Yolo"
                }
              }
            }
          }
        }
      };
    }

    // Example flow as shown in Documentation
    [HttpPost]
    public ContentResult DialogAction()
    {
      // Parse the body of the request using the Protobuf JSON parser,
      // *not* Json.NET.
      WebhookRequest request;
      using (var reader = new StreamReader(Request.Body))
      {
        request = jsonParser.Parse<WebhookRequest>(reader);
      }

      double totalAmount = 0;
      double totalNights = 0;
      double totalPersons = 0;
      if (request.QueryResult.Action == "book")
      {
        //Parse the intent params
        var requestParameters = request.QueryResult.Parameters;
        totalPersons = requestParameters.Fields["totalPersons"].NumberValue;
        totalNights = requestParameters.Fields["totalNights"].NumberValue;
        totalAmount = totalNights * 100;
      }
      // Populate the response
      WebhookResponse response = new WebhookResponse
      {
        FulfillmentText = $"Thank you for choosing our hotel, " +
        $"your total amount for the { totalNights} nights for { totalPersons} persons will be { totalAmount} USD."
      };

      // Ask Protobuf to format the JSON to return.
      // Again, we don’t want to use Json.NET — it doesn’t know how to handle Struct
      // values etc.
      string responseJson = response.ToString();
      return Content(responseJson, "application/json");
    }

    
  }
}
