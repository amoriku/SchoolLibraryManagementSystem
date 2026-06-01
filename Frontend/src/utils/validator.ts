export const validator = () => {
    const validateGrade = (gradeName: string) => {
        const regex: RegExp = /^\d{1,2}-[А-Яа-я]{1,2}/

        if (regex.test(gradeName)) {
            return true;
        }

        return false;
    }

    return {
        validateGrade
    }
}