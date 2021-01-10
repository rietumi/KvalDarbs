var viewModel = ko.mapping.fromJS(initial);

var TaskViewModel = function (model) {
    if (model) {
        return ko.mapping.fromJS(model);
    }
    else {
        var self = this;
        self.Repetition = ko.observable("");
        self.Time = ko.observable("");
        self.Exercise = ko.observable("");
    }
};

viewModel.AddTraining = function () {
    viewModel.Trainings.push({
        User: ko.observable(""),
        Tasks: ko.observableArray([new TaskViewModel(null)])
    });
};

viewModel.RemoveTraining = function (training) {
    viewModel.Trainings.remove(training);
};

viewModel.Open = function () {
    $(".exerciseToggle").toggle();
}

viewModel.CloneTraining = function (training) {
    var tasks = [];
    training.Tasks().forEach(function (t) {
        task = new TaskViewModel(ko.toJS(t));
        // New task shouldn't copy ID.
        task.TaskId = "";
        tasks.push(task);
    })

    viewModel.Trainings.push({
        User: ko.observable(training.User()),
        Tasks: ko.observableArray(tasks)
    });
}

viewModel.AddTask = function (training) {
    training.Tasks.push({
        Repetition: ko.observable(""),
        Time: ko.observable(""),
        Exercise: ko.observable({
            Name: ko.observable(""),
            Description: ko.observable("")
        })
    });
};

viewModel.RemoveTask = function (task, training) {
    training.Tasks.remove(task);
};

viewModel.Submit = function () {
    $('#teamTraining').val(ko.mapping.toJSON(viewModel));
    $('#trainingForm').submit();
}

viewModel.Exercise = {
    Name: ko.observable(""),
    Description: ko.observable("")
}

viewModel.AddExercise = function () {
    $("#exerciseAddButton").prop('disabled', true);
    $.ajax({
        method: "POST",
        url: exerciseUrl,
        data: { __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(), Name: viewModel.Exercise.Name, Description: viewModel.Exercise.Description },
        dataType: "json",
        success: function (data) {
            if (data.Key != 0) {
                viewModel.Exercises.push({
                    Key: data.Key,
                    Value: data.Value
                });
                viewModel.Exercise.Name("");
                viewModel.Exercise.Description("");
                alert("Vingrinājums veiksmīgi pievienots!");
            }
            else {
                alert(data.Value);
            }
        },
        error: function () {
            alert("Darbība neizdevās");
        },
        complete: function () {
            $("#exerciseAddButton").prop('disabled', false);
        }
    });
}

viewModel.Move = function (task, training, direction) {
    var index = training.Tasks.indexOf(task);
    if (index == -1)
        return;

    if (direction == 'up') {
        if (index == 0)
            return;

        var rawTasks = training.Tasks();
        training.Tasks.splice(index - 1, 2, rawTasks[index], rawTasks[index - 1]);
    }

    if (direction == 'down') {
        if (index == training.Tasks.length - 1)
            return;

        var rawTasks = training.Tasks();
        training.Tasks.splice(index, 2, rawTasks[index + 1], rawTasks[index]);
    }
};

$(function () {
    ko.applyBindings(viewModel);
});