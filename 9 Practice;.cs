9 Practice;

namespace AdapterPattern
{
    // Интерфейс внутренней службы доставки
    public interface IInternalDeliveryService
    {
        void DeliverOrder(string orderId);
        string GetDeliveryStatus(string orderId);
    }

    // Класс внутренней службы доставки
    public class InternalDeliveryService : IInternalDeliveryService
    {
        public void DeliverOrder(string orderId)
        {
            // Вывод сообщения о доставке внутренней службой
            Console.WriteLine($"Доставка заказа {orderId} внутренней службой.");
        }

        public string GetDeliveryStatus(string orderId)
        {
            // Возвращает статус доставки для внутренней службы
            return $"Статус внутренней доставки для заказа {orderId}.";
        }
    }

    // Внешняя служба логистики A
    public class ExternalLogisticsServiceA
    {
        public void ShipItem(int itemId)
        {
            // Вывод сообщения о доставке внешней службой A
            Console.WriteLine($"Доставка товара {itemId} через ExternalLogisticsServiceA.");
        }

        public string TrackShipment(int shipmentId)
        {
            // Возвращает информацию о доставке от внешней службы A
            return $"Информация об отслеживании для {shipmentId} от ExternalLogisticsServiceA.";
        }
    }

    // Адаптер для ExternalLogisticsServiceA
    public class LogisticsAdapterA : IInternalDeliveryService
    {
        private readonly ExternalLogisticsServiceA _externalService;

        public LogisticsAdapterA(ExternalLogisticsServiceA externalService)
        {
            _externalService = externalService;
        }

        public void DeliverOrder(string orderId)
        {
            // Конвертация orderId в целое число и вызов метода внешней службы
            _externalService.ShipItem(int.Parse(orderId));
        }

        public string GetDeliveryStatus(string orderId)
        {
            // Конвертация orderId в целое число и получение информации о статусе
            return _externalService.TrackShipment(int.Parse(orderId));
        }
    }

    // Фабрика для выбора службы доставки
    public static class DeliveryServiceFactory
    {
        public static IInternalDeliveryService GetDeliveryService(string type)
        {
            return type switch
            {
                "internal" => new InternalDeliveryService(),
                "externalA" => new LogisticsAdapterA(new ExternalLogisticsServiceA()),
                _ => null // Можно добавить другие адаптеры для других внешних служб
            };
        }
    }

    // Клиентский код
    class Program
    {
        static void Main(string[] args)
        {
            // Получаем службу доставки через фабрику
            IInternalDeliveryService deliveryService = DeliveryServiceFactory.GetDeliveryService("externalA");

            // Доставка заказа
            deliveryService?.DeliverOrder("123");

            // Получение и вывод статуса доставки
            Console.WriteLine(deliveryService?.GetDeliveryStatus("123"));
        }
    }
}
