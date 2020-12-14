using KvalDarbsCore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KvalDarbsCore.Data
{
    public class DataModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(TeamTrainingViewModel))
                return new BinderTypeModelBinder(typeof(DataModelBinder<TeamTrainingViewModel>));

            return null;
        }
    }
}
