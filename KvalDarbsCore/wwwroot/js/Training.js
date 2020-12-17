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
}

viewModel.AddTraining = function () {
    viewModel.Trainings.push({
        User: ko.observable(""),
        Tasks: ko.observableArray([new TaskViewModel(null)])
    });
};

viewModel.RemoveTraining = function (training) {
    viewModel.Trainings.remove(training);
};

viewModel.CloneTraining = function (training) {
    var tasks = [];
    training.Tasks().forEach(function (t) {
        tasks.push(new TaskViewModel(ko.toJS(t)));
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
    $('#training').val(ko.mapping.toJSON(viewModel));
    $('#trainingForm').submit();
}

$(function () {
    ko.applyBindings(viewModel);
});