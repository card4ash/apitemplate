using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApp.Helpers;

public class ModelStateErrorMessage
{
    public static string GetErrorMessage(ModelStateDictionary modelState)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var state in modelState.Values)
        {
            foreach (var err in state.Errors)
            {
                sb.Append($"{err.ErrorMessage}");
                sb.Append(Environment.NewLine);
            }
        }

        return sb.ToString();
    }
}
