using Camunda.Api.Client.ExternalTask;
using Camunda.Api.Client;
using MsgFoundation.Data;
using MsgFoundation.Models;

namespace MsgFoundation.Functions
{
    public class OrderFunctions
    {
        public static async Task CreateOrder(ExternalTaskInfo task, MsgFoundationContext dbcontext, CamundaClient camunda)
        {
            Dictionary<string, VariableValue> variables = await camunda.Executions[task.ExecutionId].LocalVariables.GetAll();
            string pedidoAsadero = variables["pedidoAsadero"].GetValue<string>();
            int cantPedidoAsadero = variables["cantPedidoAsadero"].GetValue<int>();
            string pedidoCocina = variables["pedidoCocina"].GetValue<string>();
            int cantPedidoCocina = variables["cantPedidoCocina"].GetValue<int>();
            string pedidoBar = variables["pedidoBar"].GetValue<string>();
            int cantPedidoBar = variables["cantPedidoBar"].GetValue<int>();
            DateTime currentDate = DateTime.Now.ToUniversalTime();

            Order order = new Order
            {
                Id = Guid.NewGuid(),
                CurrentDate = currentDate,
                PedidoAsadero = pedidoAsadero.ToString(),
                CantPedidoAsadero = cantPedidoAsadero,
                PedidoCocina = pedidoCocina.ToString(),
                CantPedidoCocina = cantPedidoCocina,
                PedidoBar = pedidoBar.ToString(),
                CantPedidoBar = cantPedidoBar,
            };

            dbcontext.Orders.Add(order);
            dbcontext.SaveChanges();

            FetchExternalTasks fetch = new FetchExternalTasks();
            fetch.WorkerId = "worker";
            fetch.MaxTasks = 1;
            fetch.UsePriority = true;
            fetch.Topics = new List<FetchExternalTaskTopic>();
            fetch.Topics.Add(new FetchExternalTaskTopic(task.TopicName, 1));

            List<LockedExternalTask> lockedTasks = await camunda.ExternalTasks.FetchAndLock(fetch);

            CompleteExternalTask request = new CompleteExternalTask();
            request.WorkerId = "worker";
            request.Variables = new Dictionary<string, VariableValue>();
            request.Variables.Add("orderId", VariableValue.FromObject(order.Id));

            await camunda.ExternalTasks[task.Id].Complete(request);
        }
    }
}
