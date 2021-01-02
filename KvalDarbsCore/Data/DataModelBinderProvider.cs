using KvalDarbsCore.Models;
using LogicCore;
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

            if (context.Metadata.ModelType == typeof(DateTime?) || context.Metadata.ModelType == typeof(DateTime))
                return new BinderTypeModelBinder(typeof(DateTimeModelBinder));

            return null;
        }
    }
}
