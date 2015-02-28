
angular.module('simpleLogger', [])
    .controller('logCtrl', function($scope, $http) {

        $scope.message = '';
        $scope.reply = '<server reply comes here>';
        $scope.level = 'debug';
        $scope.logger = 'main';

        $scope.log = function () {
            $scope.reply = 'thinking..';
            $http({
                method: 'POST',
                url: '/api/log',
                data: $scope.message,
                headers: {
                    'X-Logger': $scope.logger, // (optional) <debug|info|warn|error>  default:debug
                    'X-LogLevel': $scope.level // (optional) <any-string>             default:main
                }
            }).success(function(data) {
                $scope.reply = data;
                $scope.message = '';
            });
        };

        $scope.deleteLog = function() {
            $scope.reply = 'thinking..';
            $http({
                method: 'DELETE',
                url: '/api/log',
            }).success(function(data) {
                $scope.reply = data;
            });
        };

    });


