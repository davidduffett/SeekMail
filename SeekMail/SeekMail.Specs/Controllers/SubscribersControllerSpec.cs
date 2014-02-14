using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using FizzWare.NBuilder;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;
using SeekMail.Controllers;
using SeekMail.Domain;
using SeekMail.Models;

namespace SeekMail.Specs.Controllers
{
    [Subject(typeof(SubscribersController))]
    public class When_getting_subscribers : WithSubject<SubscribersController>
    {
        It should_return_all_subscriber_models = () =>
            Response.Count().ShouldEqual(SubscriberCount);

        It should_populate_all_model_fields_from_entities = () =>
        {
            for (var i = 0; i < SubscriberCount; i++)
                Response[i].ShouldBeEquivalentTo(Subscribers[i]);
        };

        Establish context = () =>
        {
            Subscribers = Builder<Subscriber>.CreateListOfSize(SubscriberCount).Build();
            The<ISubscriberRepository>().WhenToldTo(x => x.GetAll())
                .Return(Subscribers);
        };

        Because of = () =>
            Response = Subject.Get().ToList();

        static IList<Subscriber> Subscribers;
        static IList<SubscriberModel> Response;
        const int SubscriberCount = 30;
    }

    [Subject(typeof(SubscribersController))]
    public class When_posting_a_new_subscriber : WithSubject<SubscribersController>
    {
        It should_return_ok = () =>
            Response.ShouldBeOfType<OkNegotiatedContentResult<SubscriberModel>>();

        It should_add_the_new_subscriber = () =>
            The<ISubscriberRepository>().WasToldTo(x => x.Add(Param<Subscriber>.Matches(entity =>
                entity.EmailAddress == The<SubscriberModel>().EmailAddress)));

        It should_save_changes = () =>
            The<ISubscriberRepository>().WasToldTo(x => x.SaveChanges());

        Establish context = () =>
            Configure(new SubscriberModel { EmailAddress = "joe@bloggs.com" });

        Because of = () =>
            Response = Subject.Post(The<SubscriberModel>());

        static IHttpActionResult Response;
    }

    [Subject(typeof(SubscribersController))]
    public class When_posting_a_new_subscriber_and_model_state_is_invalid : WithSubject<SubscribersController>
    {
        It should_return_bad_request_with_model_state = () =>
            Response.ShouldBeOfType<InvalidModelStateResult>();

        Establish context = () =>
            Subject.ModelState.AddModelError("EmailAddress", "Value entered is invalid");

        Because of = () =>
            Response = Subject.Post(The<SubscriberModel>());

        static IHttpActionResult Response;
    }

    [Subject(typeof(SubscribersController))]
    public class When_deleting_subscribers : WithSubject<SubscribersController>
    {
        It should_delete_the_entity = () =>
            The<ISubscriberRepository>().WasToldTo(x => x.Delete(The<Subscriber>()));

        It should_save_changes = () =>
            The<ISubscriberRepository>().WasToldTo(x => x.SaveChanges());

        It should_return_ok = () =>
            Response.ShouldBeOfType<OkResult>();

        Establish context = () =>
            The<ISubscriberRepository>().WhenToldTo(x => x.Get(Id))
                .Return(The<Subscriber>());

        Because of = () =>
            Response = Subject.Delete(Id);

        static IHttpActionResult Response;
        const string Id = "joe@bloggs.com";
    }
}