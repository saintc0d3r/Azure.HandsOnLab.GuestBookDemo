using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspNetMvc_Infrastructure
{
    public static class ModelConverter
    {
        public static IEnumerable<TMvcModel> ToMvcModels<TModel, TMvcModel>(this IEnumerable<TModel> models) where TMvcModel : new()
        {
            // check parameter
            if ((models == null) || (!models.Any()))
            {
                return new TMvcModel[0];
            }

            IEnumerable<PropertyInfo> modelPropertyInfos = models.ElementAt(0).GetType().GetProperties();
            IEnumerable<PropertyInfo> mvcModelPropertyInfos = 
                typeof(TMvcModel).GetProperties().Where(propertyInfo => propertyInfo.GetCustomAttribute<MapToModelProperty>() != null);
            return 
                models.Select(model =>{
                    var mvcModel = new TMvcModel();

                    // Assign each Model's [Mapped] public property values into the MvcModel
                    foreach (var mvcModelPropertyInfo in mvcModelPropertyInfos)
                    {
                        var modelPropertyName = mvcModelPropertyInfo.GetCustomAttribute<MapToModelProperty>().ModelPropertyName;
                        var modelPropertyInfo = modelPropertyInfos.SingleOrDefault(propertyInfo => propertyInfo.Name.Equals(modelPropertyName));
                        if ((modelPropertyInfo != null) && (modelPropertyInfo.PropertyType.IsAssignableFrom(mvcModelPropertyInfo.PropertyType)))
                        {
                            var modelPropertyValue = modelPropertyInfo.GetValue(model);
                            mvcModelPropertyInfo.SetValue(mvcModel, modelPropertyValue);
                        }
                    }
                    return mvcModel;
                });
        }
    }
}