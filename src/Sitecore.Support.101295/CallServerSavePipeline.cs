using Sitecore.Caching;
using Sitecore.Data;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.ExperienceEditor.Switchers;
using Sitecore.Globalization;
using Sitecore.Pipelines;
using Sitecore.Pipelines.Save;
using System;

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.SaveItem
{
    public class CallServerSavePipeline : PipelineProcessorRequest<Sitecore.Support.ExperienceEditor.Speak.Server.Contexts.PageContext>
    {
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            PipelineProcessorResponseValue pipelineProcessorResponseValue = new PipelineProcessorResponseValue();
            Sitecore.Pipelines.Pipeline pipeline = Sitecore.Pipelines.PipelineFactory.GetPipeline("saveUI");
            pipeline.ID = Sitecore.Data.ShortID.Encode(Sitecore.Data.ID.NewID);
            Sitecore.Pipelines.Save.SaveArgs saveArgs = base.RequestContext.GetSaveArgs();
            PipelineProcessorResponseValue result;
            using (new ClientDatabaseSwitcher(base.RequestContext.Item.Database))
            {
                pipeline.Start(saveArgs);
                Sitecore.Caching.CacheManager.GetItemCache(base.RequestContext.Item.Database).Clear();
                pipelineProcessorResponseValue.AbortMessage = Sitecore.Globalization.Translate.Text(saveArgs.Error);
                result = pipelineProcessorResponseValue;
            }
            return result;
        }
    }
}