angular.module('CarFinderApp')
    .controller('CarFinderAppController', ['$scope', '$http', function ($scope, $http) {

        /* Strings */
        $scope.years = null;
        $scope.makes = null;
        $scope.models = null;
        $scope.trims = null;

        //Declare empty strings
        $scope.selectedYear = '';
        $scope.selectedMake = '';
        $scope.selectedModel = '';
        $scope.selectedTrim = '';
        $scope.carData = null;

        


        $scope.myInterval = 4000;
        $scope.wrapSliders = 4000;

        $scope.isCollapsed = true;
        $scope.isCollapsedTwo = true;

        // we use {} cause carData is a object, it holds objects.  Return one object, years,makes,models,trims
        

        $scope.getYears = function () {
            // declare options object (if necessary)
            // var options = { params: {} }; // don't need one here
            // make request - this can be from a local service or from the Angular $http service
             $http.get('api/Cars/Years').then(function (response) {
                 //assign result to a $scope variable

                $scope.years = response.data; //contain the info that I'm after

            });


            // assign result to a $scope variable

            $scope.makes = [];
            $scope.models = [];
            $scope.trims = [];
            $scope.carData = [];

        };
        $scope.getMakes = function () {
            $scope.selectedMake = '';
            $scope.selectedModel = '';
            $scope.selectedTrim = '';

            var options = { params: {year: $scope.selectedYear} }; //pass the selected year to the API

            $http.get('api/Cars/Makes', options).then(function (response) {
                $scope.makes = response.data;
            });

            $scope.models = [];
            $scope.trims = [];
            $scope.carData = [];

            };

        $scope.getModels = function () {

            var options = { params: { year: $scope.selectedYear, make: $scope.selectedMake } }; //pass the selected year to the API
            $http.get('api/Cars/Models', options).then(function (response) {
                $scope.models = response.data;
            });
            
            $scope.trims = [];
            $scope.carData = [];

        };
        $scope.getTrims = function () {

            var options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel, trim: $scope.selectedTrim } }; //pass the selected year to the API
            $http.get('api/Cars/Trims', options).then(function (response) {

                //if not equal to null
                
                    $scope.getCar();
                
               $scope.trims = response.data;
               // $scope.getCar();
            });
      
            $scope.carData = [];
        };

        $scope.getCar = function () {
            var options = {};

            if ($scope.selectedYear === "") {
                sweetAlert("Please select a year");

                $('#getCarInfo').modal();
                
            }
            else if ($scope.selectedMake === "") {
                sweetAlert("Please select a make");
                $('#getCarInfo').modal();
            }
            else if ($scope.selectedModel === "") {
                sweetAlert("Please select a model");
                $('#getCarInfo').modal();
            }
            
            else if ($scope.selectedTrim === "") {
                options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel} };
               
            }
            else {
                options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel, trim: $scope.selectedTrim } };
            }



            $http.get('api/Cars/Cars', options).then(function (response) {
                $scope.carData = response.data;
            });
        };

        $scope.getYears();

    }]);

