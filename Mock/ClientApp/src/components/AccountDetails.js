"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var react_redux_1 = require("react-redux");
var AccountStore = require("../store/AccountReducer");
var AccountData = /** @class */ (function (_super) {
    __extends(AccountData, _super);
    function AccountData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    // This method is called when the component is first added to the document
    AccountData.prototype.componentDidMount = function () {
        this.ensureDataFetched();
    };
    AccountData.prototype.render = function () {
        return (React.createElement(React.Fragment, null,
            React.createElement("h1", { id: "tabelLabel" }, "Account Summary"),
            React.createElement("p", null,
                "Total debits. ",
                this.props.account.totalDebits),
            React.createElement("p", null,
                "Total credits. ",
                this.props.account.totalCredits),
            this.renderAccountTable()));
    };
    AccountData.prototype.ensureDataFetched = function () {
        this.props.requestAccount();
    };
    AccountData.prototype.renderAccountTable = function () {
        return (React.createElement("table", { className: 'table table-striped', "aria-labelledby": "tabelLabel" },
            React.createElement("thead", null,
                React.createElement("tr", null,
                    React.createElement("th", null, "Date"),
                    React.createElement("th", null, "Balance"))),
            React.createElement("tbody", null, this.props.account.endOfDayBalances.map(function (data) {
                return React.createElement("tr", null,
                    React.createElement("td", null, data.date),
                    React.createElement("td", null, data.balance));
            }))));
    };
    return AccountData;
}(React.PureComponent));
exports.default = react_redux_1.connect(function (state) { return state.account; }, // Selects which state properties are merged into the component's props
AccountStore.actionCreators // Selects which action creators are merged into the component's props
)(AccountData); // eslint-disable-line @typescript-eslint/no-explicit-any
//# sourceMappingURL=AccountDetails.js.map