using System;
using System.Collections.Generic;

enum ProjectStatus
{
    Проект,
    Исполнение,
    Закрыт
}

enum TaskStatus
{
    Назначена,
    ВРаботе,
    НаПроверке,
    Выполнена
}

class Person
{
    public string Name 
    { 
        get;
        set; 
    }

    public Person(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}

class Report
{
    public string Text 
    { 
        get;
        private set;
    }
    public DateTime CompletionDate 
    { 
        get; 
        private set; 
    }
    public Person Performer 
    { 
        get; 
        private set; 
    }

    public Report(string text, Person performer)
    {
        Text = text;
        Performer = performer;
        CompletionDate = DateTime.Now;
    }

    public void Update(string text)
    {
        Text = text;
        CompletionDate = DateTime.Now;
    }

    public override string ToString()
    {
        return "Отчёт: " + Text + "\nДата: " + CompletionDate + "\nИсполнитель: " + Performer.Name;
    }
}

class TaskItem
{
    public string Description  
    { 
        get; 
        set; 
    }
    public DateTime Deadline 
    { 
        get; 
        set; 
    }
    public Person Initiator 
    { 
        get; 
        private set; 
    }
    public Person Performer 
    { 
        get; 
        private set; 
    }
    public TaskStatus Status 
    { 
        get; 
        private set; 
    }
    public Report TaskReport 
    { 
        get; 
        private set; 
    }

    public TaskItem(string description, DateTime deadline, Person initiator, Person performer)
    {
        Description = description;
        Deadline = deadline;
        Initiator = initiator;
        Performer = performer;
        Status = TaskStatus.Назначена;
        TaskReport = null;
    }

    // Принять задачу в работу
    public void StartWork(Person person)
    {
        if (Performer != person || Status != TaskStatus.Назначена)
        {
            Console.WriteLine("Нельзя принять эту задачу.");
            return;
        }

        Status = TaskStatus.ВРаботе;
        Console.WriteLine(person.Name + " взял задачу в работу.");
    }

    // Делегировать задачу другому человеку
    public void DelegateTask(Person currentPerformer, Person newPerformer)
    {
        if (Performer != currentPerformer || (Status != TaskStatus.Назначена && Status != TaskStatus.ВРаботе) || TaskReport != null)
        {
            Console.WriteLine("Нельзя делегировать задачу.");
            return;
        }

        Performer = newPerformer;
        Status = TaskStatus.Назначена;

        Console.WriteLine(currentPerformer.Name +
                          " делегировал задачу " + newPerformer.Name);
    }

    // Отклонить задачу
    public void RejectTask(Person person)
    {
        if (Performer != person || (Status != TaskStatus.Назначена && Status != TaskStatus.ВРаботе) || TaskReport != null)
        {
            Console.WriteLine("Нельзя отклонить задачу.");
            return;
        }

        Performer = null;
        Status = TaskStatus.Назначена;

        Console.WriteLine(person.Name + " отклонил задачу.");
    }

    // Создать отчёт
    public void CreateReport(Person person, string text)
    {
        if (Performer != person || Status != TaskStatus.ВРаботе || TaskReport != null)
        {
            Console.WriteLine("Нельзя создать отчёт.");
            return;
        }

        TaskReport = new Report(text, person);
        Status = TaskStatus.НаПроверке;

        Console.WriteLine("Отчёт отправлен на проверку.");
    }

    // Изменить существующий отчёт после возврата
    public void UpdateReport(Person person, string text)
    {
        if (Performer != person || Status != TaskStatus.ВРаботе || TaskReport == null)
        {
            Console.WriteLine("Нельзя изменить отчёт.");
            return;
        }

        TaskReport.Update(text);
        Status = TaskStatus.НаПроверке;

        Console.WriteLine("Исправленный отчёт отправлен на проверку.");
    }

    // Проверка отчёта инициатором
    public void ReviewReport(Person reviewer, bool approved)
    {
        if (reviewer != Initiator || Status != TaskStatus.НаПроверке || TaskReport == null)
        {
            Console.WriteLine("Нельзя проверить этот отчёт.");
            return;
        }

        if (approved)
        {
            Status = TaskStatus.Выполнена;
            Console.WriteLine("Отчёт утверждён. Задача выполнена.");
        }
        else
        {
            Status = TaskStatus.ВРаботе;
            Console.WriteLine("Отчёт возвращён на доработку.");
        }
    }

    // Повторное назначение отклонённой задачи
    public void Reassign(Person newPerformer)
    {
        if (Performer != null || Status != TaskStatus.Назначена || TaskReport != null)
        {
            Console.WriteLine("Нельзя переназначить задачу.");
            return;
        }

        Performer = newPerformer;
        Console.WriteLine("Задача назначена: " + newPerformer.Name);
    }

    public override string ToString()
    {
        string performerName = Performer == null ? "Не назначен" : Performer.Name;

        return "Задача: " + Description + "\nСрок: " + Deadline.ToShortDateString() + "\nИнициатор: " + Initiator.Name + "\nИсполнитель: " + performerName + "\nСтатус: " + Status;
    }
}

class Project
{
    public string Description 
    { 
        get; 
        set; 
    }
    public DateTime StartDate 
    { 
        get; 
        set; 
    }
    public DateTime EndDate 
    { 
        get; 
        set; 
    }
    public Person Customer 
    { 
        get; 
        set; 
    }
    public Person TeamLead 
    { 
        get; 
        private set; 
    }
    public List<TaskItem> Tasks 
    { 
        get; 
        private set; 
    }
    public ProjectStatus Status 
    { 
        get; 
        private set; 
    }

    public Project(string description, DateTime startDate, DateTime endDate, Person customer, Person teamLead)
    {
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Customer = customer;
        TeamLead = teamLead;

        Tasks = new List<TaskItem>();
        Status = ProjectStatus.Проект;
    }

    // Создание задачи
    public void AddTask(Person teamLead, string description, DateTime deadline, Person performer)
    {
        if (teamLead != TeamLead || Status != ProjectStatus.Проект)
        {
            Console.WriteLine("Нельзя создать задачу.");
            return;
        }

        TaskItem task = new TaskItem(description, deadline, teamLead, performer);

        Tasks.Add(task);

        Console.WriteLine("Задача создана: " + description);
    }

    // Начать исполнение проекта
    public void StartProject()
    {
        if (Status != ProjectStatus.Проект || Tasks.Count == 0)
        {
            Console.WriteLine("Нельзя начать проект.");
            return;
        }

        foreach (TaskItem task in Tasks)
        {
            if (task.Performer == null)
            {
                Console.WriteLine("Не все задачи назначены.");
                return;
            }
        }

        Status = ProjectStatus.Исполнение;
        Console.WriteLine("Проект перешёл в исполнение.");
    }

    // Переназначить задачу
    public void ReassignTask(Person teamLead, TaskItem task, Person newPerformer)
    {
        if (teamLead != TeamLead || !Tasks.Contains(task))
        {
            Console.WriteLine("Нет прав на переназначение.");
            return;
        }

        task.Reassign(newPerformer);
    }

    // Удалить отклонённую задачу
    public void DeleteTask(Person teamLead, TaskItem task)
    {
        if (teamLead != TeamLead || !Tasks.Contains(task))
        {
            Console.WriteLine("Нет прав на удаление.");
            return;
        }

        if (task.Performer != null || task.Status != TaskStatus.Назначена)
        {
            Console.WriteLine("Можно удалить только отклонённую задачу.");
            return;
        }

        Tasks.Remove(task);
        Console.WriteLine("Задача удалена.");
    }

    // Проверить отчёт
    public void ReviewTask(TaskItem task, Person reviewer, bool approved)
    {
        if (!Tasks.Contains(task) || Status != ProjectStatus.Исполнение)
        {
            Console.WriteLine("Нельзя проверить задачу.");
            return;
        }

        task.ReviewReport(reviewer, approved);

        CheckProjectStatus();
    }

    // Автоматическое закрытие проекта
    private void CheckProjectStatus()
    {
        if (Tasks.Count == 0)
            return;

        foreach (TaskItem task in Tasks)
        {
            if (task.Status != TaskStatus.Выполнена)
                return;
        }

        Status = ProjectStatus.Закрыт;
        Console.WriteLine("\nВсе задачи выполнены!");
        Console.WriteLine("Проект закрыт.");
    }

    // Вывод информации о проекте
    public void ShowProject()
    {
        Console.WriteLine("ПРОЕКТ");
        Console.WriteLine("Описание: " + Description);
        Console.WriteLine("Заказчик: " + Customer.Name);
        Console.WriteLine("Тимлид: " + TeamLead.Name);
        Console.WriteLine("Начало: " + StartDate.ToShortDateString());
        Console.WriteLine("Окончание: " + EndDate.ToShortDateString());
        Console.WriteLine("Статус: " + Status);

        Console.WriteLine("ЗАДАЧИ");

        foreach (TaskItem task in Tasks)
        {
            Console.WriteLine(task);
        }
    }
}

class Program
{
    static void Main()
    {
        List<Person> team = new List<Person>();

        for (int i = 1; i <= 10; i++)
        {
            team.Add(new Person("Сотрудник " + i));
        }

        Person teamLead = team[0];
        Person customer = new Person("Заказчик");

        Project project = new Project("Разработка Task Manager", DateTime.Today, DateTime.Today.AddMonths(2), customer, teamLead);

        for (int i = 0; i < team.Count; i++)
        {
            project.AddTask(teamLead, "Задача номер " + (i + 1), DateTime.Today.AddDays(10 + i), team[i]);
        }

        project.StartProject();

        foreach (TaskItem task in project.Tasks)
        {
            Person performer = task.Performer;

            task.StartWork(performer);

            task.CreateReport(performer, "Работа по задаче выполнена.");

            project.ReviewTask(task, teamLead, true);
        }

        project.ShowProject();
    }
}