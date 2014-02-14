using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using FizzWare.NBuilder;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;
using SeekMail.Controllers;
using SeekMail.Domain;
using SeekMail.Models;

namespace SeekMail.Tests.Controllers
{
    [Subject(typeof(TemplatesController))]
    public class When_getting_templates : WithSubject<TemplatesController>
    {
        It should_return_all_template_models = () =>
            Response.Count().ShouldEqual(TemplateCount);

        It should_populate_all_model_fields_from_entities = () =>
        {
            for (var i = 0; i < TemplateCount; i++)
                Response[i].ShouldBeEquivalentTo(Templates[i]);
        };

        Establish context = () =>
        {
            Templates = Builder<MessageTemplate>.CreateListOfSize(TemplateCount).Build();
            The<IMessageTemplateRepository>().WhenToldTo(x => x.GetAll())
                .Return(Templates);
        };

        Because of = () =>
            Response = Subject.Get().ToList();

        static IList<MessageTemplate> Templates; 
        static IList<MessageTemplateModel> Response;
        const int TemplateCount = 30;
    }

    [Subject(typeof (TemplatesController))]
    public class When_getting_a_single_message_template : WithSubject<TemplatesController>
    {
        It should_return_ok = () =>
            Response.ShouldNotBeNull();

        It should_return_the_model_matching_the_entity = () =>
            Response.Content.ShouldBeEquivalentTo(The<MessageTemplate>());

        Establish context = () =>
        {
            Configure(Builder<MessageTemplate>.CreateNew().Build());
            The<IMessageTemplateRepository>().WhenToldTo(x => x.Get(Id))
                .Return(The<MessageTemplate>());
        };

        Because of = () =>
            Response = Subject.Get(Id) as OkNegotiatedContentResult<MessageTemplateModel>;

        static OkNegotiatedContentResult<MessageTemplateModel> Response;
        const int Id = 133;
    }

    [Subject(typeof(TemplatesController))]
    public class When_getting_a_single_message_template_and_id_does_not_exist : WithSubject<TemplatesController>
    {
        It should_return_not_found = () =>
            Response.ShouldBeOfType<NotFoundResult>();

        Because of = () =>
            Response = Subject.Get(345);

        static IHttpActionResult Response;
    }

    [Subject(typeof (TemplatesController))]
    public class When_posting_a_new_message_template : WithSubject<TemplatesController>
    {
        It should_return_created = () =>
            Response.ShouldBeOfType<CreatedNegotiatedContentResult<MessageTemplateModel>>();

        It should_add_the_new_entity = () =>
            The<IMessageTemplateRepository>().WasToldTo(x => x.Add(Param<MessageTemplate>.Matches(entity =>
                entity.Name == The<MessageTemplateModel>().Name &&
                entity.Subject == The<MessageTemplateModel>().Subject &&
                entity.HtmlBody == The<MessageTemplateModel>().HtmlBody &&
                entity.TextBody == The<MessageTemplateModel>().TextBody)));

        It should_save_changes = () =>
            The<IMessageTemplateRepository>().WasToldTo(x => x.SaveChanges());

        Establish context = () =>
        {
            Configure(new MessageTemplateModel
            {
                Name = "New template",
                Subject = "Welcome to Seek",
                TextBody = "Text part of the message",
                HtmlBody = "<html></html>"
            });

            // Guff for mocking UrlHelper
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/templates");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "templates" } });
            Subject.ControllerContext = new HttpControllerContext(config, routeData, request);
            Subject.Request = request;
            Subject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            Subject.Url = new UrlHelper(request);
        };

        Because of = () =>
            Response = Subject.Post(The<MessageTemplateModel>());

        static IHttpActionResult Response;
    }

    [Subject(typeof (TemplatesController))]
    public class When_posting_a_new_template_and_model_state_is_invalid : WithSubject<TemplatesController>
    {
        It should_return_bad_request_with_model_state = () =>
            Response.ShouldBeOfType<InvalidModelStateResult>();

        Establish context = () =>
            Subject.ModelState.AddModelError("Name", "Field is required");

        Because of = () =>
            Response = Subject.Post(The<MessageTemplateModel>());

        static IHttpActionResult Response;
    }

    [Subject(typeof (TemplatesController))]
    public class When_putting_changes_to_an_existing_message_template : WithSubject<TemplatesController>
    {
        It should_update_entity_properties_to_match_input_model = () =>
            The<MessageTemplate>().ShouldBeEquivalentTo(The<MessageTemplateModel>(), options => options
                .Excluding(x => x.Created) // Created should never be updated
                .Excluding(x => x.Id)); // Id is only used to find the entity

        It should_save_changes = () =>
            The<IMessageTemplateRepository>().WasToldTo(x => x.SaveChanges());

        It should_return_ok = () =>
            Response.ShouldBeOfType<OkResult>();

        Establish context = () =>
        {
            Configure(new MessageTemplate
            {
                Id = Id,
                Created = DateTime.Now,
                Name = "Old name",
                Subject = "Old subject",
                TextBody = "Old text body",
                HtmlBody = "Old html body"
            });
            Configure(new MessageTemplateModel
            {
                Name = "New name",
                Subject = "New subject",
                TextBody = "New text body",
                HtmlBody = "New text body"
            });
            The<IMessageTemplateRepository>().WhenToldTo(x => x.Get(Id))
                .Return(The<MessageTemplate>());
        };

        Because of = () =>
            Response = Subject.Put(Id, The<MessageTemplateModel>());

        static IHttpActionResult Response;
        const int Id = 191;
    }

    [Subject(typeof (TemplatesController))]
    public class When_putting_changes_to_a_non_existent_message_template : WithSubject<TemplatesController>
    {
        It should_return_not_found = () =>
            Response.ShouldBeOfType<NotFoundResult>();

        Because of = () =>
            Response = Subject.Put(123, new MessageTemplateModel());

        static IHttpActionResult Response;
    }

    [Subject(typeof (TemplatesController))]
    public class When_putting_changes_and_model_state_is_invalid : WithSubject<TemplatesController>
    {
        It should_return_bad_request_with_model_state = () =>
            Response.ShouldBeOfType<InvalidModelStateResult>();

        Establish context = () =>
            Subject.ModelState.AddModelError("Name", "Field is required");

        Because of = () =>
            Response = Subject.Put(123, The<MessageTemplateModel>());

        static IHttpActionResult Response;
    }

    [Subject(typeof (TemplatesController))]
    public class When_deleting_message_template : WithSubject<TemplatesController>
    {
        It should_delete_the_entity = () =>
            The<IMessageTemplateRepository>().WasToldTo(x => x.Delete(The<MessageTemplate>()));

        It should_save_changes = () =>
            The<IMessageTemplateRepository>().WasToldTo(x => x.SaveChanges());

        It should_return_ok = () =>
            Response.ShouldBeOfType<OkResult>();

        Establish context = () =>
            The<IMessageTemplateRepository>().WhenToldTo(x => x.Get(Id))
                .Return(The<MessageTemplate>());

        Because of = () =>
            Response = Subject.Delete(Id);

        static IHttpActionResult Response;
        const int Id = 191;
    }
}