import toast from "react-hot-toast";

export const validator = () => {
    const validateGrade = (gradeName: string) => {
        const regex: RegExp = /^\d{1,2}-[А-Яа-я]{1,2}/

        if (regex.test(gradeName)) {
            return true;
        }

        return false;
    }

    const validateDate = (date: string, minDateOffset?: number, maxDateOffset?: number): boolean => {
        const currentDate = new Date();
        const dateToValidate = new Date(date);
        
        if (dateToValidate < currentDate) {
            return false;
        }

        return true;
    }

    const validateSelect = (selectValue: any) => {
        if (selectValue === "None")
        {
            return false;
        }

        return true;
    }

    return {
        validateGrade,
        validateDate,
        validateSelect
    }
}
